
const baseApi = 'https://commentservice.azurewebsites.net';

export const environment = {
  production: true,
  title: 'Production Environment Heading',
  apiURL: baseApi,
  allowAnonymousURL: [
    baseApi + '/api/Account/register',
    baseApi + '/api/Account/login',
    baseApi + '/api/Account/forgotPassword',
    baseApi + '/api/Account/resetPassword',
    baseApi + '/api/Account/refresh-token',
    baseApi + '/api/Comment/getAllComments',
    baseApi + '/api/Comment/getCommentById'
  ],
  topicURL: "http://test.com"
};

// ng serve --configuration=production
// ng build --configuration=production
