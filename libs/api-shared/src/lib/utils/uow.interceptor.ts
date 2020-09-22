import { Injectable, NestInterceptor, ExecutionContext, CallHandler, INestApplicationContext, UseInterceptors, createParamDecorator, Inject } from "@nestjs/common";
import { from, Observable, of } from "rxjs";
import { catchError, mapTo, switchMap, tap } from "rxjs/operators";
import { MongoClient } from "mongodb";
import { ClientSession, Connection } from "mongoose";
import { GqlExecutionContext } from '@nestjs/graphql';
import { InjectConnection } from '@nestjs/mongoose';
import { CustomParamFactory } from '@nestjs/common/interfaces';
import { GraphQLResolveInfo } from 'graphql';
import { mongoose } from '@typegoose/typegoose';



@Injectable()
export class UnitOfWorkInterceptor implements NestInterceptor {
  /**
   *
   */
  constructor(@Inject("DATABASE_CONNECTION") private readonly mongooseInstance: typeof mongoose) {

  }
  async intercept(context: ExecutionContext, next: CallHandler): Promise<Observable<any>> {
    const ctx = GqlExecutionContext.create(context).getContext();
    let requestId = ctx.req.headers["requestId"];
    if (!requestId) {
      throw new Error("request must have header of requestId");
    }
    let uow = await UnitOfWork.getOrCreateUow(requestId, this.mongooseInstance.connection);
    ctx.uow = uow;

    return next.handle().pipe(
      switchMap(data =>
        from(uow.commit()).pipe(mapTo(data))
      ),
      tap(() => uow.mongoSession.inTransaction() && uow.mongoSession.endSession()),
      catchError(async err => {
        await uow.abort()
        throw err;
      })
    );
  }
}

export class UnitOfWork {

  static uowMap: Map<string, UnitOfWork> = new Map();
  static async getOrCreateUow(requestId: string, connection: Connection) {
    let uow = UnitOfWork.uowMap.get(requestId);
    if (!uow) {
      uow = await new UnitOfWork().init(requestId, connection);
      UnitOfWork.uowMap.set(requestId, uow);
    }
    return uow;
  }
  mongoSession: ClientSession;
  connection: Connection;
  id: string;
  async init(uowId: string, connection: Connection) {
    this.id = uowId;
    this.connection = connection;
    this.mongoSession = await this.connection.startSession();
    this.connection["session"] = this.mongoSession;
    this.mongoSession.startTransaction();
    return this;
  }

  async abort() {
    this.mongoSession.inTransaction() && await this.mongoSession.abortTransaction();
    this.mongoSession.endSession();
  }

  async commit() {
    this.mongoSession.inTransaction() && await this.mongoSession.commitTransaction();
    this.mongoSession.endSession();
  }
}

export const Uow = UseInterceptors.bind(null, UnitOfWorkInterceptor) as () => MethodDecorator;
export const mongoSessionFactory: CustomParamFactory = (
  data: any,
  ctx: ExecutionContext
) => GqlExecutionContext.create(ctx).getContext().uow;
export const CurrentUow = createParamDecorator(mongoSessionFactory);
