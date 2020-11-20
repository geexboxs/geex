import { readFileSync } from "fs";
import { StringDecoder } from "string_decoder";
import { join } from "path";
import { IGeexServerConfig } from "@geex/api-shared";
import * as json5 from 'json5';
export const Env = process.env.GEEX_ENV ?? "dev";//todo: 读取环境变量
export const AppConfig: IGeexServerConfig & { [key: string]: any } = {
  "production": false,
  "hostname": "127.0.0.1",
  "port": 4000,
  "connections": {
    "mongo": "mongodb://u5ern%40me:P%40ssw0rd@localhost:27017/dev?authSource=admin",
    "redis": "redis://localhost:6379/1"
  },
  "authConfig": {
    "tokenSecret": "dev",
    "expiresIn": 86400
  },
  "loggerConfig": {},
  "traceConfig": {
    "serviceName": "geex",
    "sampler": {
      "type": "const",
      "param": 1
    },
    "reporter": {
      "logSpans": true,
      "collectorEndpoint": "http://localhost:14268/api/traces"
    }
  }
};

// export const AppConfig$ = (async () => {
//   AppConfig = await import(`../configs/${Env}.json`)
// })();
