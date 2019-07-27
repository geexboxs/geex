import { User } from '../../domain/models/user.model';

export interface Context {
    user?: User;
}
