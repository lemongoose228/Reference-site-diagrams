<template>
	<div class="content">
			<header>
				<router-link to="/">
					<span class="nav">Выйти</span>
				</router-link>
				<router-link to="/article/create">
					<span class="nav">Создать статью</span>
				</router-link>
			</header>
			<h1 class="main_title title">Типы диаграмм справочник</h1>
			<div class="filter-buttons">
				<button type="button" class="filter-button" data-category="time" data-color="active-blue">
					<input v-model="tags" class="filter_checkbox" type="checkbox" value="time">
					Измерение во времени
				</button>
				<button type="button" class="filter-button" data-category="part" data-color="active-yellow">
					<input v-model="tags" class="filter_checkbox" type="checkbox" value="part">
					Целое на части
				</button>
				<button type="button" class="filter-button" data-category="compare" data-color="active-pink">
					<input v-model="tags" class="filter_checkbox" type="checkbox" value="compare">
					Сравнение
				</button>
				<button type="button" class="filter-button" data-category="relation" data-color="active-orange">
					<input v-model="tags" class="filter_checkbox" type="checkbox" value="relation">
					Отношения
				</button>

				<button @click="searching" type="button" class="search_button">Найти</button>
			</div>
			<div class="loader_wrapper">
				<span v-if="loading" class="loader"></span>
			</div>

			<div id="articles">

			</div>
	</div>
		
</template>

<script lang="js">
		import { defineComponent } from 'vue';
		import hiddenIconUrl from "@/assets/hidden.png";
		import imgDeleteUrl from "@/assets/bin.png";
		import imgEditUrl from "@/assets/ink-pen.png";

		export default defineComponent({
				data() {
						return {
								loading: true,
								articles: [],
								tags: []
						}
				},
				created() {

				},
				async mounted() {
						if (!sessionStorage.getItem('isAdmin')) {
								this.$router.push('/');
						}

						this.loading = true;
						this.articles = await this.getAllArticles();
						

						this.createArticles(this.articles);
						this.loading = false;

						const filterButtons = document.querySelectorAll('.filter-button');
						const activeCategories = new Set();
						filterButtons.forEach(button => {
								button.addEventListener('click', () => {
										const category = button.dataset.category;
										const colorClass = button.dataset.color;
						
										if (activeCategories.has(category)) {
												activeCategories.delete(category);
												button.classList.remove(colorClass);
												button.style.backgroundColor = '#555';
										} else {
												activeCategories.add(category);
												button.classList.add(colorClass);
												button.style.backgroundColor = '';
										}
						
								});
						});
				},
				methods: {
						createArticles(articles) {
								let article_container = document.getElementById("articles");
								article_container.innerHTML = "";

								articles.forEach(article => {

										let div = document.createElement("div");
										div.classList.add("card");
										div.setAttribute("articleId", article.id);

										div.addEventListener("click", (e) => {
												let div_parent = e.target.closest(".card");
												console.log(div_parent);

												if (div_parent) {
														
														localStorage.setItem("articleId", div_parent.getAttribute("articleId"));

														this.$router.push('/article');
												}
										});

										let inner_div = document.createElement("div");
										inner_div.classList.add("markdown_container");

										inner_div.innerHTML = marked.parse(article.shortDescription);

										let cardButtonWrapper = document.createElement("div");
										cardButtonWrapper.classList.add("cardButtonWrapper");

										let tagWrapper = document.createElement("div");
										tagWrapper.classList.add("tagWrapper");

										article.tags.forEach(tag => {
												let tagElement = document.createElement("div");
												tagElement.classList.add("tag");
												if (tag === "time") {
														tagElement.style.backgroundColor = "skyblue";
												}
												if (tag === "part") {
														tagElement.style.backgroundColor = "yellow";
												}
												if (tag === "compare") {
														tagElement.style.backgroundColor = "deeppink";
												}
												if (tag === "relation") {
														tagElement.style.backgroundColor = "orange";
												}

												tagWrapper.append(tagElement);
										});

										if (!article.publish) {
												let hiddenIcon = document.createElement("img");
												hiddenIcon.classList.add("hiddenIcon");
											hiddenIcon.src = hiddenIconUrl;
												cardButtonWrapper.append(hiddenIcon);
										}

										let buttonDelete = document.createElement("div");
										buttonDelete.classList.add("buttonDelete");

										let imgDelete = document.createElement("img");
										imgDelete.src = imgDeleteUrl;
										imgDelete.classList.add("imgDelete");
										buttonDelete.append(imgDelete);

										let buttonEdit = document.createElement("div");
										buttonEdit.classList.add("buttonEdit");

										buttonDelete.addEventListener("click", async (e) => {
												e.stopPropagation();
												let div_parent = e.target.closest(".card");

												if (div_parent) {
														const url = 'https://localhost:48867/api/delete-article';
														const body = JSON.stringify({ 
																Id: parseInt(div_parent.getAttribute("articleId"))
														});
						
														const res = await fetch(url, {
																method: 'DELETE',
																headers: {
																		'Content-Type': 'application/json'
																},
																body: body
														});
												
														try {
																if (!res.ok) { 
																		throw new Error(`Ошибка сервера: ${res.status}`);
																}

																this.$router.go(0);
												
														} catch (error) {
																const errorText = await res.text();
																throw new Error(`HTTP Error: ${res.status} - ${res.statusText}, Body: ${errorText}`);
														}
												}
										});

										buttonEdit.addEventListener("click", (e) => {
												e.stopPropagation();
												let div_parent = e.target.closest(".card");

												if (div_parent) {
														
														localStorage.setItem("articleId", div_parent.getAttribute("articleId"));

														this.$router.push('/article/edit');
												}
										});

										let imgEdit = document.createElement("img");
										imgEdit.src = imgEditUrl;
										imgEdit.classList.add("imgEdit");
										buttonEdit.append(imgEdit);

										cardButtonWrapper.prepend(tagWrapper);
										cardButtonWrapper.append(buttonEdit);
										cardButtonWrapper.append(buttonDelete);
										div.append(cardButtonWrapper);
										div.append(inner_div);

										if (!article.publish) {
												article_container.prepend(div);
										}
										else {
												article_container.append(div);
										}
								});
						},
						async getAllArticles() {
							const url = 'https://localhost:48867/api/get-all-articles-teacher';

								const res = await fetch(url, {
										method: 'GET'
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
						},
						async searching() {
							const url = 'https://localhost:48867/api/get-articles-by-tag';
								const body = JSON.stringify({ 
										tags: this.tags,
										isTeacher: true
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
										this.createArticles(data);
						
								} catch (error) {
										const errorText = await res.text();
										throw new Error(`HTTP Error: ${res.status} - ${res.statusText}, Body: ${errorText}`);
								}
						}
				}
		});
</script>


<style scoped>

		.content {
				width: 1280px;
				margin: 0 auto;
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
				height: 350px;
				margin-bottom: 20px;
				border-radius: 10px;
				overflow: hidden;
				background-color: #333;
				box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
				margin-left: 10px;  /* Added left margin */
				margin-right: 10px; /* Added right margin */
				padding: 15px;  
				cursor: pointer;
		}

		.card .markdown_container img {
				width: 100%;
				height: 200px;
				object-fit: cover;
		}

		.markdown_container {
				color:#fff;
				height: 80%;
				overflow: hidden;
				display: flex;
				flex-direction: column;
				align-items: flex-start;
		}

		.markdown_container p {
				margin: 4px;
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
				justify-content: space-around;
				padding-top: 24px;
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

  #articles {
    width: 100%;
    margin-top: 40px;
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    justify-content: center;
  }

		.main_title {
				text-align: center;
				margin: 40px 0px;
		}

		.title {
				color: #fff;
		}

		.buttonEdit, .buttonDelete, .hiddenIcon {
				padding: 8px;
		}

		.buttonEdit:hover, .buttonDelete:hover {
				background-color: #555;
				border-radius: 6px;
		}

		.imgDelete {
				width: 24px;
				height: 24px;
		}

		.imgEdit {
				width: 24px;
				height: 24px;
		}

		.hiddenIcon {
				width: 24px;
				height: 24px;
		}

		.cardButtonWrapper {
				display: flex;
				flex-direction: row;
				justify-content: flex-end;
				gap: 8px;
				width: 100%;
		}

		.search_button {
				padding: 10px 20px;
				border-radius: 20px;
				border: 1px solid #888; /* Default inactive border */
				background-color: transparent; /* Default inactive background */
				color: #fff;
				cursor: pointer;
				transition: all 0.3s ease; /* Smooth transition for hover effect */
				font-size: 16px;
		}

		.search_button:hover {
				background-color: #9966FF; /* Active background color (Purple) */
				border-color: #9966FF; /* Active border color (Purple) */
				color: #fff;
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

		.tag {
				width: 16px;
				height: 16px;
				border-radius: 100px;
		}

		.tagWrapper {
				display: flex;
				gap: 8px;
				width: 100%;
		}
</style>
