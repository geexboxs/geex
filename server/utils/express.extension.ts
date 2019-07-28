import { GraphQLModule, GraphQLModuleOptions } from "@graphql-modules/core";
import { IResolvers } from "@kamilkisiela/graphql-tools";
import { application } from "express";
import accountsBoost from "@accounts/boost";
import { ApolloServer } from "apollo-server-express";
import bodyParser = require("body-parser");
import { ExpressToken, ApolloToken } from "../tokens";
import { buildSchemaSync } from "type-graphql";
import { RoleBasedAuthChecker } from "../../shared/authentication/role-based-auth-checker";
import { globalLoggingMiddleware } from "../../shared/logging/globalLogging.middleware";
type GeexGraphqlServerConfig = GraphQLModuleOptions<any, any, any, IResolvers<any, any>> & {
    useAccounts?: boolean;
};

declare module "express-serve-static-core" {
    interface Express {
        useGeexGraphql(this: Express, config: GeexGraphqlServerConfig): Promise<Express>;
    }
}

application["useGeexGraphql"] = async function useGeexGraphql(this, config: GeexGraphqlServerConfig) {
    this.use(bodyParser())
        .use(globalLoggingMiddleware)
    config.providers = config.providers === undefined ? [] : config.providers as []
    config.imports = config.imports === undefined ? [] : config.imports as []

    config.providers.push({
        provide: ExpressToken,
        useValue: this
    })
    const entryModule = new GraphQLModule(config);

    //其他的模块初始化逻辑


    // 在其他模块之上统一加上auth
    const accounts = (await accountsBoost({
        tokenSecret: 'terrible secret',
        // micro: true, // setting micro to true will instruct `@accounts/boost` to only verify access tokens without any additional session logic
    }));
    const accountsModule = accounts.graphql();
    config.imports.push(<GraphQLModule<any, any, any, IResolvers<any, any>>><unknown>accountsModule)
    config.extraSchemas = [buildSchemaSync({
        authChecker: RoleBasedAuthChecker,
        resolvers: [entryModule.resolvers],
        container: ({ ...args }) => {
            return entryModule.injector.getSessionInjector(args.context)
        },
        // container: ({ context }) => context.injector

    })]

    // 根据entryModule生成 graphql server
    const apollo = new ApolloServer({
        typeDefs: entryModule.typeDefs,
        schema: entryModule.schema,
        // @ts-ignore
        schemaDirectives: {
            // In order for the `@auth` directive to work
            ...entryModule.schemaDirectives,
        },
        context: ({ req }) => entryModule.context({ req }),
        resolvers: entryModule.resolvers,
    });
    // 将 graphql server 挂载到 express
    apollo.applyMiddleware({ app: this })

    // 注入 graphql server 到 entryModule
    entryModule.selfProviders.push({
        provide: ApolloToken,
        useValue: apollo
    })

    return this;
}

// extraSchemas: [
//     buildSchemaSync({
//         authChecker: authChecker,
//         resolvers: [],
//         container: ({ ...args }) => {
//             return UserModule.injector.getSessionInjector(args.context)
//         },
//         // container: ({ context }) => context.injector

//     })
// ],
//     providers: [
//         {
//             provide: GeexLogger, useValue: new GeexLogger({})
//         },
//     ],
//         typeDefs: [
//             ...OKGScalarDefinitions,
//         ],
