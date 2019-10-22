
import { GraphQLModule } from "@graphql-modules/core";
import { ProviderScope } from "@graphql-modules/di";
import acl from "acl";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";
import { createConnection } from "mongoose";
import { environment } from "../../environments/environment";
import { IGeexContext, IGeexServerConfig, IAuthConfig } from "../../types";
import { AclToken } from "./tokens";
import { AccountsServer } from "@accounts/server";
import { AccountsPassword } from "@accounts/password";
import { Mongo } from "@accounts/mongo";
import { AccountsModule } from "@accounts/graphql-api";
import { DatabaseManager } from "@accounts/database-manager";
import { GeexServerConfigToken } from "../tokens";

// Build a storage for storing users
const accountsMongo = new Mongo(createConnection(environment.connectionString));
// Create database manager (create user, find users, sessions etc) for accounts-js
const accountsDb = new DatabaseManager({
    sessionStorage: accountsMongo,
    userStorage: accountsMongo,
});
// Create accounts server that holds a lower level of all accounts operations
const accountsServer = new AccountsServer(
    {
        db: accountsDb,
        tokenSecret: environment.authConfig.tokenSecret,
        ambiguousErrorMessages: false, // change to `true` to hide user id,
    },
    {
        password: new AccountsPassword(),
    },
);

export const AuthModule = new GraphQLModule<IAuthConfig, ExpressContext, IGeexContext>({
    defaultProviderScope: ProviderScope.Application,
    providers: () => [
        // {
        //     provide: PasswordHasher,
        //     useFactory: (injector) => new PasswordHasher(injector.get<IAuthConfig>(AuthConfigToken) && injector.get<IAuthConfig>(AuthConfigToken).tokenSecret || ""),
        // },
        // {
        //     provide: Passport,
        //     useFactory: (injector) => passport.use("local", new LocalStrategy(
        //         (username, password, done) => {
        //             UserModel.findOne({ username }, (err, user) => {
        //                 if (err) { return done(err); }
        //                 if (!user) { return done(null, false); }
        //                 if (injector.get(PasswordHasher).hash(password) !== user.passwordHash) { return done(null, false); }
        //                 return done(null, user);
        //             });
        //         },
        //     )),
        // },
        {
            provide: AclToken,
            useFactory: (injector) => new acl(new acl.mongodbBackend(createConnection(injector.get<IGeexServerConfig>(GeexServerConfigToken).connectionString).db, "")),
        },
        // {
        //     provide: UserModelToken,
        //     useValue: UserModel,
        // }, AuthResolver,
    ],
    // extraSchemas: () => [
    //     buildSchemaSync({
    //         resolvers: [AuthResolver],
    //         container: {
    //             get: (someClass, resolverData) => {
    //                 return (resolverData.context as IGeexContext).injector.get(someClass);
    //             },
    //         },
    //     }),
    // ],
    imports: [AccountsModule.forRoot({
        accountsServer,
    })],
}, environment.authConfig);
// Creates resolvers, type definitions, and schema directives used by accounts-js
// export const AuthModule = AccountsModule.forRoot({
//     accountsServer,
// });
