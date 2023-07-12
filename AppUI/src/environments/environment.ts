
const baseApi = 'https://localhost:7200';

export const environment = {
  production: false,
  title: 'Local Environment Heading',
  apiURL: baseApi,
  allowAnonymousURL: [
    baseApi + '/api/Account/register',
    baseApi + '/api/Account/login',
    baseApi + '/api/Account/refresh-token',
    baseApi + '/api/Comment/getAllComments',
    baseApi + '/api/Comment/getCommentById'
  ]
};

// ng serve
