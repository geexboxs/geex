
import { Inject } from "@graphql-modules/di";
import { Authorized, Resolver, Query, Mutation } from "type-graphql";
import { AuditLog } from "./audit-log.model";

@Resolver(of => AuditLog)
export class AuditLogResolver {

    constructor(

    ) { }

    @Query(returns => AuditLog)
    async log() {
        return new AuditLog("testLog");
    }
}
