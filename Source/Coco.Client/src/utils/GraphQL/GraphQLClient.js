// Graphql
import { ApolloClient } from "apollo-client";
import { createHttpLink } from "apollo-link-http";
import { setContext } from "apollo-link-context";
import { InMemoryCache } from "apollo-cache-inmemory";
import { AUTH_KEY, AUTH_USER_HASHED_ID } from "../../utils/AppSettings";

const authHttpLink = createHttpLink({
  uri: process.env.REACT_APP_AUTH_API_URL
});

const authLink = setContext(async (_, { headers }) => {
  return {
    headers: {
      ...headers
    }
  };
});

const authClient = new ApolloClient({
  link: authLink.concat(authHttpLink),
  cache: new InMemoryCache(),
  ssrMode: true,
  defaultOptions: {
    watchQuery: {
      fetchPolicy: "cache-and-network",
      errorPolicy: "ignore"
    },
    query: {
      fetchPolicy: "network-only",
      errorPolicy: "all"
    },
    mutate: {
      errorPolicy: "all"
    }
  }
});

const authorizedHttpLink = createHttpLink({
  uri: process.env.REACT_APP_AUTH_API_URL
});

const authorizedLink = setContext(async (_, { headers }) => {
  const token = localStorage.getItem(AUTH_KEY);
  const userHash = localStorage.getItem(AUTH_USER_HASHED_ID);
  return {
    headers: {
      ...headers,
      authorization: token ? token : "",
      "x-header-user-hash": userHash
    }
  };
});

const authorizedClient = new ApolloClient({
  link: authorizedLink.concat(authorizedHttpLink),
  cache: new InMemoryCache(),
  ssrMode: true,
  defaultOptions: {
    watchQuery: {
      fetchPolicy: "cache-and-network",
      errorPolicy: "ignore"
    },
    query: {
      fetchPolicy: "network-only",
      errorPolicy: "all"
    },
    mutate: {
      errorPolicy: "all"
    }
  }
});

export { authClient, authorizedClient };
