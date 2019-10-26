// /*!
//  * Connect - Redis
//  * Copyright(c) 2012 TJ Holowaychuk <tj@vision-media.ca>
//  * MIT Licensed
//  */
// import expressSession = require("express-session");
// import { Request } from "express";
// import json5 = require("json5");

// // All callbacks should have a noop if none provided for compatibility
// // with the most Redis clients.
// const noop = () => { };

// export class RedisSessionStore extends expressSession.Store {
//     prefix: any;
//     scanCount: number;
//     serializer: any;
//     client: any;
//     ttl: any;
//     disableTouch: any;
//     constructor(options: Partial<RedisSessionStore> = {}) {
//         super(options);
//         if (!options.client) {
//             throw new Error("A client must be directly provided to the RedisStore");
//         }

//         this.prefix = options.prefix || "session";
//         this.scanCount = Number(options.scanCount) || 100;
//         this.serializer = options.serializer || json5;
//         this.client = options.client;
//         this.ttl = options.ttl || 86400; // One day in seconds.
//         this.disableTouch = options.disableTouch || false;
//     }

//     get = (sid: string, callback: (err: any, session?: Express.SessionData | null) => void) => {
//         const key = this.prefix + sid;

//         this.client.get(key, (err, data) => {
//             if (err) {
//                 throw err;
//             }
//             let result;
//             try {
//                 result = this.serializer.parse(data);
//             } catch (err) {
//                 throw err;
//             }
//             return callback(null, result);
//         });
//     }

//     set = (sid: string, session: Express.SessionData, callback: (err?: any) => void = noop) => {
//         const args = [this.prefix + sid];

//         let value;
//         try {
//             value = this.serializer.stringify(session);
//         } catch (er) {
//             return callback(er);
//         }
//         args.push(value);
//         args.push("EX", this._getTTL(session));

//         this.client.set(args, callback);
//     }

//     touch = (sid: string, session: Express.SessionData, callback: (err?: any, result?: string) => void = noop) => {
//         if (this.disableTouch) { return callback(); }

//         const key = this.prefix + sid;
//         this.client.expire(key, this._getTTL(session), (err, ret) => {
//             if (err) { return callback(err); }
//             if (ret !== 1) { return callback(null, "EXPIRED"); }
//             callback(null, "OK");
//         });
//     }

//     destroy = (sid: string, callback: Function = noop) => {
//         const key = this.prefix + sid;
//         this.client.del(key, callback);
//     }

//     clear = (callback: Function = noop) => {
//         this._getAllKeys((err, keys) => {
//             if (err) { return callback(err); }
//             this.client.del(keys, callback);
//         });
//     }

//     length = (callback: Function = noop) => {
//         this._getAllKeys((err, keys) => {
//             if (err) { return callback(err); }
//             return callback(null, keys.length);
//         });
//     }

//     ids = (callback: Function = noop) => {
//         const prefixLen = this.prefix.length;

//         this._getAllKeys((err, keys) => {
//             if (err) { return callback(err); }
//             keys = keys.map(key => key.substr(prefixLen));
//             return callback(null, keys);
//         });
//     }

//     all = (callback: Function = noop) => {
//         const prefixLen = this.prefix.length;

//         this._getAllKeys((err, keys) => {
//             if (err) { return callback(err); }
//             if (keys.length === 0) { return callback(null, []); }

//             // tslint:disable-next-line: variable-name
//             this.client.mget(keys, (_err, sessions) => {
//                 if (_err) { return callback(_err); }

//                 let result;
//                 try {
//                     result = sessions.map((data, index) => {
//                         data = this.serializer.parse(data);
//                         data.id = keys[index].substr(prefixLen);
//                         return data;
//                     });
//                 } catch (e) {
//                     _err = e;
//                 }
//                 return callback(_err, result);
//             });
//         });
//     }

//     _getTTL(session) {
//         let ttl;
//         if (session && session.cookie && session.cookie.expires) {
//             const ms = Number(new Date(session.cookie.expires)) - Date.now();
//             ttl = Math.ceil(ms / 1000);
//         } else {
//             ttl = this.ttl;
//         }
//         return ttl;
//     }

//     _getAllKeys(callback: Function = noop) {
//         const pattern = this.prefix + "*";
//         this._scanKeys({}, 0, pattern, this.scanCount, callback);
//     }

//     _scanKeys(keys = {}, cursor, pattern, count, callback: Function = noop) {
//         const args = [cursor, "match", pattern, "count", count];
//         this.client.scan(args, (err, data) => {
//             if (err) { return callback(err); }

//             const [nextCursorId, scanKeys] = data;
//             for (const key of scanKeys) {
//                 keys[key] = true;
//             }

//             // This can be a string or a number. We check both.
//             if (Number(nextCursorId) !== 0) {
//                 return this._scanKeys(keys, nextCursorId, pattern, count, callback);
//             }

//             callback(null, Object.keys(keys));
//         });
//     }
// }
