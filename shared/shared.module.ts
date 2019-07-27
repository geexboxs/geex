import { GraphQLModule } from "@graphql-modules/core";
import { GeexLogger } from "./logging/logger";

export const SharedModule = new GraphQLModule({
    providers: [
        {
            provide: GeexLogger, useValue: new GeexLogger({})
        },
    ]
});
