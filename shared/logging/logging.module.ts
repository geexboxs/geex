import { GraphQLModule } from "@graphql-modules/core";
import { GlobalLoggingExtension } from "./global-logging.gql-extension";
import { GeexLogger, LoggerConfig } from "./Logger";
import { RequestIdentityExtension } from "./request-identity.gql-extension";
import { Injector } from "@graphql-modules/di";
import { LoggerConfigToken } from "./tokens";

export const LoggingModule = new GraphQLModule<LoggerConfig | undefined>({
    providers: [GlobalLoggingExtension, {
        provide: GeexLogger,
        useFactory: (injector: Injector) => new GeexLogger(injector.get(LoggerConfigToken))
    }, RequestIdentityExtension],
    imports: []
});
