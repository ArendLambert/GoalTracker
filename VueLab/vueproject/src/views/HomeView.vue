<template>fdkmpfm</template>

<script setup>
import { onMounted } from "vue";
import router from "../router/script";
onMounted(() => {
  console.log("HomeView mounted");
  router.push({ path: "/registerpage" });
});
</script>

<style scoped></style>
