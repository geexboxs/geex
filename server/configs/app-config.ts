import { IGeexServerConfig } from "./types";

export const appConfig: IGeexServerConfig = {
    hostname: "127.0.0.1",
    port: 4000,
    connections: {
        mongo: `mongodb://${encodeURIComponent("u5ern@me")}:${encodeURIComponent("P@ssw0rd")}@localhost:27017/test?authSource=admin`,
        redis: "redis://localhost:6379/1",
        smtp: undefined,
        // smtp: {
        //     host: "smtp-mail.outlook.com",
        //     port: 587,
        //     username: "geex@outlook.com",
        //     password: "P@ssw0rd",
        //     secure: false,
        //     sendAs: {
        //         name: "geex@outlook.com",
        //         address: "geex@outlook.com",
        //     },
        // },
    },
    authConfig: { tokenSecret: "test", expiresIn: 3600 * 24 },
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
