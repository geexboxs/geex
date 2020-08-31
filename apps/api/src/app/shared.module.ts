import { Module, Provider } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';
import { User } from './account/models/user.model';
import { getModelForClass, mongoose } from '@typegoose/typegoose';
import { Role } from './user-manage/model/role.model';
import { environments } from '../configs/app-config';
import { PassportModule } from '@nestjs/passport';
import { JwtModule } from '@nestjs/jwt';
import { EmailSender } from '../shared/utils/email-sender';
import { UserClaims } from './account/models/user-claim.model';
import { PermissionScalar } from './authorization/scalars/permission.scalar';
import { CqrsModule } from '@nestjs/cqrs';
import { SessionStore } from './authentication/models/session.model';
import ioredis = require("ioredis");
import { AccessControl } from '@geexbox/accesscontrol';
import { GeexServerConfigToken } from '../shared/tokens';
import { GeexLogger } from '../shared/utils/logger';

const REEXPORTS = [
    PassportModule,
    CqrsModule,
    JwtModule.register({
        secret: environments.authConfig.tokenSecret,
        signOptions: { expiresIn: environments.authConfig.expiresIn },
    }),
    MongooseModule.forRoot(environments.connections.mongo, {
        useNewUrlParser: true,
        useUnifiedTopology: true,
        connectionFactory: (connection) => {
            connection.plugin(require('mongoose-autopopulate'));
            return connection;
        },
    }),
    MongooseModule.forFeature([
        { name: nameof(User), schema: getModelForClass(User).schema },
        { name: nameof(UserClaims), schema: getModelForClass(UserClaims).schema },
    ])];

const PROVIDERS: Provider[] = [
    GeexLogger,
    {
        provide: GeexServerConfigToken,
        useValue: environments,
    },
    PermissionScalar,
    SessionStore,
    {
        provide: ioredis,
        useValue: new ioredis(environments.connections.redis),
    },
    {
        provide: "rbac",
        useFactory: async () => (await mongoose.connect(environments.connections.mongo)).connection.createCollection("rbac", { autoIndexId: false }),
    },
    {
        provide: AccessControl,
        inject: ["rbac"],
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

@Module({
    imports: [...REEXPORTS],
    providers: [...PROVIDERS],
    exports: [...REEXPORTS, ...PROVIDERS],
})
export class SharedModule { }
