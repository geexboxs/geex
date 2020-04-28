import React, { useState } from 'react';
import { ApolloProvider, ApolloClient, HttpLink, InMemoryCache } from '@apollo/client';

const Layout: React.FC = ({ children }) => {
  const [client, setClient] = useState(
    new ApolloClient({
      link: new HttpLink({
        uri: 'http://localhost:4000/graphql',
        // headers: {
        //   Authorization: `Bearer ${authToken}`
        // }
      }),
      cache: new InMemoryCache(),
    }),
  );

  return (
    <ApolloProvider client={client}>
      <>{children}</>
    </ApolloProvider>
  );
};

export default Layout;
