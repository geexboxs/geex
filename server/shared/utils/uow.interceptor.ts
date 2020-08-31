import { Injectable, NestInterceptor, ExecutionContext, CallHandler, INestApplicationContext, UseInterceptors } from "@nestjs/common";
import { Observable, of } from "rxjs";
import { MongoClient } from "mongodb";
import { ClientSession } from "mongoose";



@Injectable()
export class UnitOfWorkInterceptor implements NestInterceptor {
    /**
     *
     */
    constructor() {

    }
    async intercept(context: ExecutionContext, next: CallHandler): Promise<Observable<any>> {
        let requestId = context.req.header("requestId");
        if (!requestId) {
            throw new Error("request must have header of requestId");
        }
        let uow = UnitOfWork.tryInitUow(requestId, context.injector);
        try {
            let result = next.handle();
            await uow.commit();
            return result;
        } catch (error) {
            await uow.abort();
            return of(error);
        }
    }
}

export class UnitOfWork {

    static uowMap: Map<string, UnitOfWork> = new Map();
    static tryInitUow(requestId: string, context: INestApplicationContext) {
        let uow = UnitOfWork.uowMap.get(requestId);
        if (!uow) {
            uow = new UnitOfWork(requestId, context);
            UnitOfWork.uowMap.set(requestId, uow);
        }
        return uow;
    }
    mongoSession: ClientSession;
    context: INestApplicationContext;
    id: string;
    constructor(uowId: string, context: INestApplicationContext) {
        this.id = uowId;
        this.context = context;
        this.mongoSession = this.context.get(MongoClient).startSession();
    }

    async abort() {
        await this.mongoSession.abortTransaction();
    }

    async commit() {
        await this.mongoSession.commitTransaction();
    }
}

export const Uow = UseInterceptors.bind(UnitOfWorkInterceptor) as () => MethodDecorator;
