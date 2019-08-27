import { Injector } from '@graphql-modules/di';
import { User } from '../domain/models/user.model';

export interface GeexContext {
    user?: User;
    injector: Injector;
}
