import { User } from '../domain/models/user.model';
import { Injector } from '@graphql-modules/di';

export interface GeexContext {
    user?: User;
    injector: Injector;
}
