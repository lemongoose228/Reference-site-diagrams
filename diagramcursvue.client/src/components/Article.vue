<template>
  <div class="container">
    <header>
      <router-link to="/teacher">
        <span class="nav" id="teacher">Выйти</span>
      </router-link>
      <router-link to="/student">
        <span style="display: none;" class="nav" id="student">Выйти</span>
      </router-link>
    </header>
    <h1 class="main_title title">Статья</h1>
    <div id="article">
      <div class="loader_wrapper">
        <span v-if="loading" class="loader"></span>
      </div>
    </div>
  </div>

</template>

<script lang="js">
    import { defineComponent } from 'vue';

    export default defineComponent({
        data() {
            return {
                loading: false
            }
        },
        async mounted() {
            
            let teacher_exit = document.getElementById("teacher");
            let student_exit = document.getElementById("student");

            if (sessionStorage.getItem('isAdmin') === "true") {
                teacher_exit.style.display = "block";
                student_exit.style.display = "none";
            }
            else {
                teacher_exit.style.display = "none";
                student_exit.style.display = "block";
            }

            this.loading = true;
            let data = await this.getArticleById();
            this.loading = false;

            let article_container = document.getElementById("article");

            article_container.innerHTML = marked.parse(data.articleText);
        },
        methods: {
            async getArticleById() {
                const url = 'https://localhost:48867/api/get-article-by-id';
                const body = JSON.stringify({ 
                    Id: parseInt(localStorage.getItem("articleId"))
                });

                const res = await fetch(url, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: body
                });
            
                try {
                    if (!res.ok) { 
                        throw new Error(`Ошибка сервера: ${res.status}`);
                    }
            
                    const data = await res.json();
                    return data;
            
                } catch (error) {
                    const errorText = await res.text();
                    throw new Error(`HTTP Error: ${res.status} - ${res.statusText}, Body: ${errorText}`);
                }
            }
        }
    });
</script>


<style scoped>
    body {
        font-family: sans-serif;
        background-color: #222;
        margin: 0;
        padding: 0;
        color: #fff;
    }

    .container {
        max-width: 1200px;
        margin: 0 auto;
        padding: 20px;
    }

    .container  {
        color: #fff;
    }

    .filter-buttons {
        display: flex;
        justify-content: center;
        margin-bottom: 20px;
    }

    .filter-button {
        padding: 10px 20px;
        margin: 0 10px;
        border: none;
        border-radius: 20px;
        font-size: 16px;
        cursor: pointer;
        background-color: #555;
        color: #fff;
        position: relative;
    }

    .filter-button.active-blue {
        background-color: skyblue;
        color: #000;
    }

    .filter-button.active-yellow {
        background-color: yellow;
        color: #000;
    }

    .filter-button.active-pink {
        background-color: deeppink;
        color: #fff;
    }

    .filter-button.active-orange {
        background-color: orange;
        color: #000;
    }

    .card-container {
        display: flex;
        flex-wrap: wrap;
        justify-content: center;
    }

    .card {
        width: 300px;
        margin-bottom: 20px;
        border-radius: 10px;
        overflow: hidden;
        background-color: #333;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        margin-left: 10px;  /* Added left margin */
        margin-right: 10px; /* Added right margin */
    }

    .card img {
        width: 100%;
        height: 200px;
        object-fit: cover;
    }

    .card-content {
        padding: 15px;
    }

    .card-title {
        font-size: 1.2em;
        margin-bottom: 10px;
    }

    .card-description {
        font-size: 0.9em;
        color: #ddd;
    }

    .category-indicators {
        display: flex;
        padding: 5px;
    }

    .category-indicator {
        width: 12px;
        height: 12px;
        border-radius: 50%;
        margin-right: 5px;
    }

    .category-indicator.time {
        background-color: skyblue;
    }

    .category-indicator.part {
        background-color: yellow;
    }

    .category-indicator.compare {
        background-color: deeppink;
    }

    .category-indicator.relation {
        background-color: orange;
    }

    header {
        display: flex;
        flex-direction: row;
        padding-top: 24px;
        /* justify-content: space-around; */
    }

  .nav {
    padding: 10px 20px;
    border-radius: 20px;
    border: 1px solid #888; /* Default inactive border */
    background-color: transparent; /* Default inactive background */
    color: #fff;
    cursor: pointer;
    transition: all 0.3s ease; /* Smooth transition for hover effect */
    font-size: 16px;
  }

    .nav:hover {
      background-color: #9966FF; /* Active background color (Purple) */
      border-color: #9966FF; /* Active border color (Purple) */
      color: #fff;
    }

    .editor_wrapper {
        background-color: #fff;
        margin-bottom: 36px;
    }

    .title {
        color: #fff;
    }

    .checkbox_wrapper {
        display: flex;
        flex-direction: row;
        gap: 12px;
    }

    .publish_checkbox {
        width: 32px;
    }

    .edit_article_submit {
        padding: 14px 24px;
        border-radius: 20px;
        border: 1px solid #888; /* Default inactive border */
        background-color: transparent; /* Default inactive background */
        color: #fff;
        cursor: pointer;
        transition: all 0.3s ease; /* Smooth transition for hover effect */
        font-size: 18px;
        margin-top: 16px;
        margin-bottom: 32px;
    }

    .edit_article_submit:hover {
        background-color: #9966FF; /* Active background color (Purple) */
        border-color: #9966FF; /* Active border color (Purple) */
        color: #fff;
    }

    .edit_article_submit_wrapper {
        display: flex;
        flex-direction: row;
        justify-content: end;
    }

    .main_title {
        text-align: center;
    }

    .filter_checkbox {
        position: absolute;
        width: 100%;
        height: 100%;
        opacity: 0;
        cursor: pointer;
        left: 0px;
        top: 0px;
    }

    .article * {
    color: #fff;
    }

    .loader_wrapper {
        display: flex;
        flex-direction: row;
        justify-content: center;
    }

    .loader {
        width: 48px;
        height: 48px;
        display: inline-block;
        position: relative;
        border: 3px solid;
        border-color:#de3500 #0000 #fff #0000;
        border-radius: 50%;
        box-sizing: border-box;
        animation: 1s rotate linear infinite;
        ;
    }
    .loader:before , .loader:after{
        content: '';
        top: 0;
        left: 0;
        position: absolute;
        border: 10px solid transparent;
        border-bottom-color:#fff;
        transform: translate(-10px, 19px) rotate(-35deg);
    }
    .loader:after {
        border-color: #de3500 #0000 #0000 #0000 ;
        transform: translate(32px, 3px) rotate(-35deg);
    }
    @keyframes rotate {
        100%{    transform: rotate(360deg)}
    }
</style>
