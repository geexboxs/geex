import { Module, Provider, Global } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';
import { User } from './account/models/user.model';
import { getModelForClass, mongoose } from '@typegoose/typegoose';
import { Role } from './user-manage/model/role.model';
import { AppConfig } from '@geex/api/app/app_config';
import { PassportModule } from '@nestjs/passport';
import { JwtModule } from '@nestjs/jwt';
import { UserClaims } from './account/models/user-claim.model';
import { PermissionScalar } from './authorization/scalars/permission.scalar';
import { CqrsModule } from '@nestjs/cqrs';
import { SessionStore } from './authentication/models/session.model';
import * as ioredis  from "ioredis";
import { AccessControl } from '@geexbox/accesscontrol';
import { GeexLogger, GeexServerConfigToken, LoggerConfigToken, Rbac } from '@geex/api-shared';

const REEXPORTS = [
  PassportModule,
  CqrsModule,
  JwtModule.register({
    secret: AppConfig.authConfig.tokenSecret,
    signOptions: { expiresIn: AppConfig.authConfig.expiresIn },
  }),
  MongooseModule.forRoot(AppConfig.connections.mongo, {
    useNewUrlParser: true,
    useUnifiedTopology: true,
    connectionFactory: (connection) => {
      connection.plugin(require('mongoose-autopopulate'));
      return connection;
    },
  }),
  MongooseModule.forFeature([
    { name: User.name, schema: getModelForClass(User).schema },
    { name: UserClaims.name, schema: getModelForClass(UserClaims).schema },
  ])];

const PROVIDERS: Provider[] = [
  {
    provide: LoggerConfigToken,
    useValue: AppConfig.loggerConfig,
  },
  {
    provide: GeexServerConfigToken,
    useValue: AppConfig,
  },
  GeexLogger,
  PermissionScalar,
  SessionStore,
  {
    provide: ioredis,
    useValue: new ioredis(AppConfig.connections.redis),
  },
  {
    provide: Rbac,
    useFactory: async () => (await mongoose.connect(AppConfig.connections.mongo)).connection.collection("rbac", { autoIndexId: false }),
  },
  {
    provide: AccessControl,
    inject: [Rbac],
    useFactory: async (store: mongoose.Collection) => new AccessControl(Object.fromEntries((await store.find().toArray()).map(x => {
      let obj = { ...x };
      delete obj["_id"];
      return [x["_id"], obj];
    })), (grants) => setTimeout(async () => {
      let ops = Object.entries(grants).map(x => {
        return {
          updateOne: {
            filter: { _id: x[0] },
            update: x[1],
            upsert: true,
          },
        };
      });
      if (ops && ops.length) {
        await store.bulkWrite(ops);
      }
    }, 1)),
  },
]

@Global()
@Module({
  imports: [...REEXPORTS],
  providers: [...PROVIDERS],
  exports: [...REEXPORTS, ...PROVIDERS],
})
export class SharedModule { }
