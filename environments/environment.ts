import { IGeexServerConfig as IServerConfig, ILoggerConfig, IAuthConfig } from "../types";

export const environment: IServerConfig = {
    hostname: "127.0.0.1",
    port: 4000,
    connections: {
        mongo: "mongodb://XxusernamexX:XxpasswordxX@localhost:27017/test?authSource=admin",
        redis: "redis://localhost:6379/0?password=P@ssw0rd",
        smtp: {
            host: "smtp-mail.outlook.com",
            port: 587,
            username: "snys98@outlook.com",
            password: "snys19931103",
            secure: false,
            sendAs: {
                name: "snys98@outlook.com",
                address: "snys98@outlook.com",
            },
        },
    },
    authConfig: { tokenSecret: "test" },
    loggerConfig: {},
    traceConfig: {
        serviceName: "geex",
        sampler: {
            type: "const",
            param: 1,
        },
        reporter: {
            logSpans: true,
            collectorEndpoint: "http://localhost:14268/api/traces",
        },
    },
};
