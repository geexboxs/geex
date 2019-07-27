import {User} from "../../domain/models/user.model";
import { ExpressContext } from "apollo-server-express/dist/ApolloServer";

export interface Context extends ExpressContext {
     currentUser: User;
}
