<template>
  <v-container>
    <div class="grid-layout">
      <div class="header">
        <v-text-field
            v-model="inputSearch"
            label="Search"
            required
        ></v-text-field>
      </div>
      <div class="chats">
        <v-list shaped>
          <v-list-item
              v-for="(item, i) in topics"
              :key="i"
              :value="item"
              active-color="primary"
              rounded="shaped"
              @click="openTopic(item)"
          >
            <v-list-item-avatar start>
              <v-icon :icon="item.Icon"></v-icon>
            </v-list-item-avatar>
            <v-list-item-title v-text="item.Text"></v-list-item-title>
          </v-list-item>
        </v-list>
      </div>
      <div class="main">
        <v-list shaped>
          <v-list-item
              v-for="(item, i) in messages"
              :key="i"
              :value="item"
              active-color="primary"
              rounded="shaped"
              :ref="el => handleScroll(el, i)"
          >            
            <v-list-item-title v-bind:class="{right : ownMessage(item)}" v-text="item.Content"></v-list-item-title>
          </v-list-item>
        </v-list>
      </div>
      <div class="bottom">
        <v-text-field
            v-model="inputMessage"
            label="Enter Your Message..."
            required
            v-on:keyup.enter="submit()"
        ></v-text-field>
      </div>
    </div>
  </v-container>
</template>

<script setup lang="ts">

import {ref, onMounted, watch, onBeforeUpdate} from "vue";
import {useQuery, useSubscription} from "villus";
import {USER_ID} from "../constants/keys";
import {Topic} from "../models/topic";
import {Message} from "../models/message";

let selectedTopic = ref<string>('')
let topics = ref<Array<Topic>>([]);
let messages = ref<Array<Message>>([])
let inputMessage = ref<string>('')
let inputSearch = ref<string>('')
let scrollElement = ref();

let subscribedTopics: string[] = [];

const getTopics = async () : Promise<Array<Topic>> => {
  const topicsQuery = `
  {
    topics(topicType: DIRECT){
    id
    participants{
      id
      topicId
      userId
    }
    topicType
    topicOwner
    }
  }
  `

  const topicResponse = await useQuery({query: topicsQuery});
  const topics = topicResponse.data.value.topics
  const userId = localStorage.getItem(USER_ID);

  return topics.map((item) => {
    let screenName = item.participants.filter(x => x.userId !== userId).map(y => y.userId).join(',')
    return new Topic(screenName, 'mdi-account', item.id)
  })
};

const getMessages = async (item: Topic) : Promise<Array<Message>> => {
  const messagesQuery = `
  {
    messages(topicId: "${item.TopicId}"){
    id,
    topicId,
    senderId,
    content,
    timestamp
    }
  }
  `
  const messagesResponse = await useQuery({query: messagesQuery});
  const msgs = messagesResponse.data.value.messages;
  return msgs.map((item) => {
    return new Message(item.id, item.topicId, item.senderId, item.content, item.timestamp);
  })
}

const sendMessage = async (topicId: string, content: string) =>{
  const sendMessageQuery = `
  {
    sendMessage(topicId: "${topicId}", content: "${content}") {
    id,
    topicId,
    timestamp,
    content
    }
  }
  `;
  
  const sendMessageResponse = await useQuery({query: sendMessageQuery});
  const data = sendMessageResponse.data.value;
  console.log(data);
}

function subscribeTopic(topicId: string) {
  const subscription = `
  subscription{
    newMessage(topicId: "${topicId}") {
      id
      topicId,
      senderId,
      content,
      timestamp
    }
  }
  `
  const { data } = useSubscription({query: subscription});

  watch(data, (value) => {
    const msg = value.newMessage
    if (msg.topicId === selectedTopic.value){
      const exists = messages.value.find(x => x.Id === msg.id);
      if (exists === undefined)
        messages.value.push(new Message(msg.id, msg.topicId, msg.senderId, msg.content, msg.timestamp));
    }
  });
}

async function openTopic(item: Topic){
  if (selectedTopic.value === item.TopicId)
    return;
  
  selectedTopic.value = item.TopicId;
  messages.value = [];
  
  if (!subscribedTopics.includes(item.TopicId)){
    subscribeTopic(item.TopicId);
    subscribedTopics.push(item.TopicId);
  }
  
  Object.assign(messages.value, await getMessages(item));
}

function ownMessage(item: Message) : boolean{
  const userId = localStorage.getItem(USER_ID);
  return item.SenderId == userId;
}

async function submit(){
  await sendMessage(selectedTopic.value, inputMessage.value);
  inputMessage.value = '';
}

watch(scrollElement, value => {
  if (value !== null && value.$el !== null){
    value.$el.scrollIntoView({behavior: "smooth"});
  }
})

function handleScroll(el: any, index: number){
  scrollElement.value = el;
}

onMounted(async () => {
  Object.assign(topics.value, await getTopics());
});

onBeforeUpdate(() =>{
  scrollElement.value = {};
})
</script>

<style scoped lang="scss">
  .grid-layout{
    height: 100%;
    display: grid;
    grid-template-areas: 
        "header header header"
        "chats main main"
        "chats bottom bottom";
    .header{
      grid-area: header;
      padding: 15px 0;
      .search{
        
      }
    }
    .chats{
      background-color: yellow;
      grid-area: chats;
    }
    .main{
      background-color: cornflowerblue;
      grid-area: main;
      overflow-y: scroll;
      max-height: 600px;
      
      .right{
        width: 100%;
        text-align: right;
      }
    }
    .bottom{
      grid-area: bottom;
      background-color: aqua;
    }
  }
</style>