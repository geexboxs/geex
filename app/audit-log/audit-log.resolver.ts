
import { Inject } from "@graphql-modules/di";
import { Authorized, Mutation, Query, Resolver } from "type-graphql";
import { AuditLog } from "./audit-log.model";

@Resolver((of) => AuditLog)
export class AuditLogResolver {

    @Query((returns) => AuditLog)
    public async log() {
        return new AuditLog("testLog");
    }
}
