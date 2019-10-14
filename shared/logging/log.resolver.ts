
import { Inject } from "@graphql-modules/di";
import { Authorized, Resolver, Query, Mutation } from "type-graphql";
import { Log } from "./log.model";

@Resolver(of => Log)
export class LogResolver {

    constructor(

    ) { }

    @Query(returns => Log)
    async log() {
        return new Log("testLog");
    }
}
