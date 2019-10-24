
import { GraphQLModule } from "@graphql-modules/core";
import { ProviderScope, Type } from "@graphql-modules/di";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { createConnection } from "mongoose";
import { environment } from "../../environments/environment";
import { IGeexContext, IGeexServerConfig, IAuthConfig } from "../../types";
import { AccountsServer } from "@accounts/server";
import { AccountsPassword } from "@accounts/password";
import { Mongo } from "@accounts/mongo";
import { AccountsModule } from "@accounts/graphql-api";
import { DatabaseManager } from "@accounts/database-manager";
import { GeexServerConfigToken, UserModelToken } from "../../shared/tokens";
import { Model, Enforcer, newEnforcer } from "casbin";
import MongooseAdapter from "@elastic.io/casbin-mongoose-adapter";
import { CasbinModel } from "./utils/casbin-model";
import { UserResolver } from "./user.resolver";
import { buildSchemaSync, ClassType } from "type-graphql";
import { getModelForClass, ReturnModelType } from "@typegoose/typegoose";
import { User } from "./models/user.model";
import { PasswordHasher } from "./utils/password-hasher";
import { GraphQLLocalStrategy } from "graphql-passport";

const resolvers: [ClassType] = [UserResolver];

// Build a storage for storing users
// Create database manager (create user, find users, sessions etc) for accounts-js
// Create accounts server that holds a lower level of all accounts operations
const accountsServer = new AccountsServer(
    {
        db: new DatabaseManager({
            sessionStorage: new Mongo(createConnection(environment.connectionString)),
            userStorage: new Mongo(createConnection(environment.connectionString)),
        }),
        tokenSecret: environment.userConfig.tokenSecret,
        ambiguousErrorMessages: false, // change to `true` to hide user id,
        tokenConfigs: {
            accessToken: {
                expiresIn: 3600,
            },
            refreshToken: {
                expiresIn: "30d",
            },
        },
    },
    {
        password: new AccountsPassword(),
    },
);

export const UserModule = new GraphQLModule<IAuthConfig, ExpressContext, IGeexContext>({
    providers: () => [
        {
            provide: Passport,
            useFactory: (injector) => passport.use("local", new GraphQLLocalStrategy((username, password, done) => {
                const users = injector.get<ReturnModelType<User>>(UserModelToken);
                const matchingUser = users.find(user => username === user.email && password === user.password);
                const error = matchingUser ? null : new Error('no matching user');
                done(error, matchingUser);
            }),),
        },
        {
            provide: UserModelToken,
            useFactory: (provider) => getModelForClass(User),
        },
        {
            provide: PasswordHasher,
            useFactory: (provider) => new PasswordHasher(environment.userConfig.tokenSecret),
        },
        {
            provide: Enforcer,
            useFactory: async (injector) => {
                const adapter = await MongooseAdapter.newAdapter(injector.get<IGeexServerConfig>(GeexServerConfigToken).connectionString);
                const enforcer = await newEnforcer(CasbinModel, adapter);
                return enforcer;
            },
        },
        ...resolvers,
    ],
    extraSchemas: [
        buildSchemaSync({
            container: ({ ...args }) => {
                return {
                    get(someClass, resolverData) {
                        return (resolverData.context as IGeexContext).injector.get(someClass);
                    },
                };
            },
            resolvers,
        }),
    ],
    imports: [AccountsModule.forRoot({
        accountsServer,
    })],
}, environment.userConfig);
// Creates resolvers, type definitions, and schema directives used by accounts-js
// export const AuthModule = AccountsModule.forRoot({
//     accountsServer,
// });
