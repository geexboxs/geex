import { Module, Provider, Global } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';
import { User } from './account/models/user.model';
import { getModelForClass, mongoose } from '@typegoose/typegoose';
import { Role } from './user-manage/model/role.model';
import { environment } from '@env';
import { PassportModule } from '@nestjs/passport';
import { JwtModule } from '@nestjs/jwt';
import { UserClaims } from './account/models/user-claim.model';
import { PermissionScalar } from './authorization/scalars/permission.scalar';
import { CqrsModule } from '@nestjs/cqrs';
import { SessionStore } from './authentication/models/session.model';
import ioredis = require("ioredis");
import { AccessControl } from '@geexbox/accesscontrol';
import { GeexLogger, GeexServerConfigToken, LoggerConfigToken, Rbac } from '@geex/api-shared';
import { Connection } from 'mongoose';

const REEXPORTS = [
  PassportModule,
  CqrsModule,
  JwtModule.register({
    secret: environment.authConfig.tokenSecret,
    signOptions: { expiresIn: environment.authConfig.expiresIn },
  }),

];

const PROVIDERS: Provider[] = [
  {
    provide: LoggerConfigToken,
    useValue: environment.loggerConfig,
  },
  {
    provide: GeexServerConfigToken,
    useValue: environment,
  },
  {
    provide: 'DATABASE_CONNECTION',
    useFactory: (): Promise<typeof mongoose> =>
      mongoose.connect(environment.connections.mongo, { useNewUrlParser: true }),
  },
  {
    provide: User,
    useFactory: (mongooseInstance: typeof mongoose) => {
      let model = mongooseInstance.model(User.name, getModelForClass(User).schema);
      // User.prototype = {
      //   //@ts-ignore
      //   constructor : model,
      //   ...User.prototype
      // }
      return model;
    },
    inject: ['DATABASE_CONNECTION'],
  },
  {
    provide: UserClaims,
    useFactory: (mongooseInstance: typeof mongoose) => { return mongooseInstance.model(UserClaims.name, getModelForClass(UserClaims).schema) },
    inject: ['DATABASE_CONNECTION'],
  },
  GeexLogger,
  PermissionScalar,
  SessionStore,
  {
    provide: ioredis,
    useValue: new ioredis(environment.connections.redis),
  },
  {
    provide: Rbac,
    useFactory: async (mongooseInstance: typeof mongoose) => mongooseInstance.connection.collection("rbac", { autoIndexId: false }),
    inject: ['DATABASE_CONNECTION'],
  },
  {
    provide: AccessControl,
    inject: [Rbac],
    useFactory: async (store: mongoose.Collection) => {
      let entries = [];
      try {
        entries = await store.find()?.toArray();
      } finally {
        return new AccessControl(Object.fromEntries((entries).map(x => {
          let obj = { ...x };
          delete obj["_id"];
          return [x["_id"], obj];
        })), (grants) => setTimeout(async () => {
          let ops = Object.entries(grants).map(x => {
            return {
              updateOne: {
                filter: { _id: x[0] },
                update: {
                  $set: x[1],
                },
                upsert: true,
              },
            };
          });
          if (ops && ops.length) {
            await store.bulkWrite(ops);
          }
        }, 1))
      }
    },
  },
]

@Global()
@Module({
  imports: [...REEXPORTS],
  providers: [...PROVIDERS],
  exports: [...REEXPORTS, ...PROVIDERS],
})
export class SharedModule { }
