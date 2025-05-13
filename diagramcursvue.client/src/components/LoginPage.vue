<template>
	<div class="content">
			<div class="container">
					<div class="spot"></div>
					<div class="gray-container">
						<h1>ВХОД</h1>
						<div id="choiceWindow">
							<p>Выберите режим</p>

							<div class="button-container">
								<router-link to="/student">
									<button class="mode-button">Ученик</button>
								</router-link>
								<button class="mode-button" @click="teacherClick">Учитель</button>
							</div>
						</div>
						<div id="teacherWindow">
							<button class="mode-button" @click="teacherBack">Назад</button>
							<p>Введите код</p>

							<div class="button-container">
								<form class="secret_code_form" @submit="loginTeacher">
									<input v-model="secret_code" class="secret_code" type="text" placeholder="Секретный код">
									<button type="submit" class="secret_code_button">Войти</button>
								</form>
							</div>
						</div>
					</div>
			</div>
	</div>


</template>

<script lang="js">
		import { defineComponent } from 'vue';

		export default defineComponent({
				data() {
						return {
								currentWindow: 'choice',
								secret_code: "",

						}
				},
				mounted() {
						sessionStorage.setItem('isAdmin', 'false');
				},
				methods: {
						teacherClick(event) {
								let choiceWindow = document.getElementById("choiceWindow");
								let teacherWindow = document.getElementById("teacherWindow");

								choiceWindow.style.display = "none";
								teacherWindow.style.display = "block";
						},
						teacherBack(event) {
								let choiceWindow = document.getElementById("choiceWindow");
								let teacherWindow = document.getElementById("teacherWindow");
								
								choiceWindow.style.display = "block";
								teacherWindow.style.display = "none";
						},
						loginTeacher(event) {
								event.preventDefault();
								
								console.log(this.secret_code)
								if (this.secret_code === "admin") {
										sessionStorage.setItem('isAdmin', 'true');
										this.$router.push('/teacher');
								}
						}
				}
		});
</script>


<style scoped>

		.container {
				min-width: auto;
				display: flex;
				justify-content: center;
				align-items: center;
				padding: 0px;
		}

		h1 {
				color: #9966FF; 
		}

  .content {
			width: 1280px;
			margin: 0 auto;
			height: 100%;
  }

		p {
				color:#e6e6e6;
				padding-top: 10px;
		}

		.button-container {
				display: flex;
				justify-content: space-evenly;
				gap: 20px; /* Spacing between buttons */
				margin-top: 20px;
		}

		.mode-button, .secret_code_button {
				padding: 10px 20px;
				border-radius: 20px;
				border: 1px solid #888; /* Default inactive border */
				background-color: transparent; /* Default inactive background */
				color: #fff;
				cursor: pointer;
				transition: all 0.3s ease; /* Smooth transition for hover effect */
				font-size: 16px;
		}

		/* Hover (Active) State */
		.mode-button:hover, .secret_code_button:hover {
				background-color: #9966FF; /* Active background color (Purple) */
				border-color: #9966FF; /* Active border color (Purple) */
				color: #fff;
		}


		/* Styling for the rounded gray container */
  .gray-container {
    background-color: rgba(34, 34, 34, 0.8); /* Slightly transparent dark gray */
    border-radius: 20px;
    padding: 40px;
    z-index: 2; /* Ensure the container and content are above the spots */
    position: absolute; /* Create a stacking context */
    width: 250px;
    top: 35%;
    left: 37%;
    box-shadow: 0px 5px 10px 10px rgba(255, 255, 255, 0.2);
  }

		/* "Spot" - Overlays */

		.secret_code {
				border-radius: 4px;
				width: 130px;
		}

		.secret_code_form {
				display: flex;
				flex-direction: row;
				gap: 8px;
		}

		#teacherWindow {
				display: none;
		}
</style>
