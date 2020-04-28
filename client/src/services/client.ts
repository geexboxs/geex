import { ApolloClient, HttpLink, InMemoryCache } from '@apollo/client';

export const ApolloClientInstance = new ApolloClient({
  link: new HttpLink({
    uri: 'http://localhost:4000/graphql',
  }),
  cache: new InMemoryCache(),
});
