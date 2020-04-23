import React from 'react';
import { ApolloProvider, ApolloClient, HttpLink, InMemoryCache } from '@apollo/client';

const Layout: React.FC = ({ children }) => (
  <ApolloProvider
    client={
      new ApolloClient({
        link: new HttpLink({
          uri: 'http://localhost:4000/graphql',
          // headers: {
          //   Authorization: `Bearer ${authToken}`
          // }
        }),
        cache: new InMemoryCache(),
      })
    }
  >
    <>{children}</>
  </ApolloProvider>
);

export default Layout;
