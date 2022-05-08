<template>
  <div class="form-container">
    <h3>Authenticate</h3>
    <v-form
        ref="form"
    >
      <v-text-field
          v-model="username"
          label="Username"
          required
      ></v-text-field>
  
      <v-text-field
          v-model="password"
          label="Password"
          required
      ></v-text-field>
    </v-form>
    <div class="btn-container">
      <v-btn
          flat
          color="secondary"
          @click="authenticate"
      >Login</v-btn>
      <v-btn
          flat
          color="primary"
      >Register</v-btn>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useQuery } from 'villus';
import { useRouter, useRoute } from 'vue-router'
import {USER_ID, USER_NAME, USER_ROLE, USER_TOKEN} from "../constants/keys";

const router = useRouter()
const route = useRoute()

const props = defineProps({
  username: { type: String, default: '' },
  password: { type: String, default: '' },
})


async function authenticate() {
  const returnUrl = route.query.returnUrl
  const authenticateQuery = `
    {
      authenticate(username: "${props.username}", password: "${props.password}") {
        token
        expiration
      }
    }`
  const authenticateResponse = await useQuery({query: authenticateQuery});
  
  const userToken = authenticateResponse.data.value.authenticate.token;
  localStorage.setItem(USER_TOKEN, userToken);
  
  const userInfoQuery = `
   {
    info {
      userId
      username
      role
    }
  }
  `
  const userInfoResponse = await useQuery({query: userInfoQuery})
  const userInfo = userInfoResponse.data.value.info;

  localStorage.setItem(USER_NAME, userInfo.username);
  localStorage.setItem(USER_ROLE, userInfo.role);
  localStorage.setItem(USER_ID, userInfo.userId);
  
  await router.push({path: returnUrl});
}

</script>

<style scoped lang="scss">
.form-container{
  h3{
    border-radius: 5px 5px 0 0;
    background-color: cornflowerblue;
    padding: 15px;
  }
  margin: auto;
  width: 500px;
  
  .btn-container{
    display: flex;
    justify-content: center;
    gap: 15px;
  }
}

</style>