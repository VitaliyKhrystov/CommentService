
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
  topicURL: "http://test.com",
  contacts: {
    email: "vitalii_khrystov@proton.me",
    linkedin: "https://www.linkedin.com/in/khrystov-vitalii-58946b131/",
    github: "https://github.com/VitaliyKhrystov",
    telegram: "Vitaliy Khrystov",
    address: "Ukraine, Kyiv"
  }
};

// ng serve --configuration=production
// ng build --configuration=production
