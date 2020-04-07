import { TracingConfig } from "jaeger-client";
import { LogTarget } from "../types";
import { LogLevel } from "@nestjs/common";
export interface IAuthConfig {
    tokenSecret: string;
}
export interface ILoggerConfig {
    /**
     * log target
     *
     * @type {LogTarget}
     * @default "console"
     * @memberof LoggerConfig
     */
    target?: LogTarget;
    /**
     * minimal level to log
     *
     * @type {LogLevel}
     * @default "info"
     * @memberof LoggerConfig
     */
    filterLevel?: LogLevel;
    consoleConfig?: {};
    fileConfig?: {};
    remoteConfig?: {};
    /**
     * metadata to be logged in every log entry
     *
     * @type {{}}
     * @memberof LoggerConfig
     */
    metadata?: {};
}
type ISmtpConfig = {
    secure: boolean;
    host: string;
    port: number;
    username: string;
    password: string;
    sendAs: {
        name: string;
        address: string;
    };
};
export interface IGeexServerConfig {
    hostname: string;
    port: number;
    connections: {
        mongo: string;
        redis: string;
        smtp?: ISmtpConfig;
    };
    traceConfig: TracingConfig;
    loggerConfig: ILoggerConfig;
    authConfig: IAuthConfig;
}
