import { IGeexServerConfig } from "../types";

export const environment: IGeexServerConfig = {
    hostname: "127.0.0.1",
    port: 4000,
    connectionString: "mongodb://XxusernamexX:XxpasswordxX@localhost:27017/test?authSource=admin",
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
