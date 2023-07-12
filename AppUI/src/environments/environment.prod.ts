
const baseApi = 'https://localhost:7200';

export const environment = {
  production: true,
  title: 'Production Environment Heading',
  apiURL: baseApi,
  allowAnonymousURL: [
    baseApi + '/api/Account/register',
    baseApi + '/api/Account/login',
    baseApi + '/api/Account/refresh-token',
    baseApi + '/api/Comment/getAllComments',
    baseApi + '/api/Comment/getCommentById'
  ]
};

// ng serve --configuration=production
