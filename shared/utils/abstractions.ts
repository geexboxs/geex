import { Injector } from '@graphql-modules/di';
import { User } from '../../app/user/user.model';
import { ExpressContext } from 'apollo-server-express/dist/ApolloServer';
import { LoggerConfig } from './logger';
import { AuthConfig } from '../authentication/auth.module';

export interface GeexContext {
    session: ExpressContext;
    user?: User;
    injector: Injector;
}

export type GeexServerConfig = {
    connectionString: string;
    loggerConfig?: LoggerConfig;
    authConfig?: AuthConfig;
};
