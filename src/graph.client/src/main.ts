import { createApp } from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import { loadFonts } from './plugins/webfontloader'
import router from "./router";
import {createClient, defaultPlugins, handleSubscriptions} from "villus";
import { SubscriptionClient } from 'subscriptions-transport-ws';
import {USER_TOKEN} from "./constants/keys";

function authPlugin({ opContext } : any) {
    const userToken = localStorage.getItem(USER_TOKEN);
    if (userToken !== undefined || true || userToken !== '')
        opContext.headers.Authorization = `Bearer ${userToken}`;
}

const subscriptionClient = new SubscriptionClient('wss://localhost:44327/graphql', {});
const subscriptionForwarder = (operation: any) => subscriptionClient.request(operation);
const client = createClient({
    url: 'https://localhost:44327/graphql',
    use: [handleSubscriptions(subscriptionForwarder), authPlugin, ...defaultPlugins()],
});

loadFonts()

createApp(App)
  .use(vuetify)
  .use(router)
  .use(client)
  .mount('#app')
