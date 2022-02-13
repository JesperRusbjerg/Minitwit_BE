<template>
    <div :class="[ifRegistrationForm ? 'registration-form' : 'login-form']">
        <form>
            <div v-show="ifRegistrationForm" class="username-input">
                <label for="username">Enter your name: </label>
                <input type="text" name="username" :id="ifRegistrationForm ? 'register-username' : 'login-username'" required>
            </div>
             <div class="email-input">
                <label for="email">Enter your email: </label>
                <input type="email" name="email" :id="ifRegistrationForm ? 'register-email' : 'login-email'" required>
            </div>
            <div class="password-input">
                <label for="password">Enter your password: </label>
                <input type="password" name="password" :id="ifRegistrationForm ? 'register-password' : 'login-password'" required>
            </div>
            <div class="submit-btn">
                <input type="submit" value="Submit" @click="sendRequest(ifRegistrationForm)">
            </div>
        </form>
    </div>
</template>
<script>
import { computed, inject } from "vue";

export default {
    name: "UserForm",
    props: {
        ifRegistrationForm: {
            type: Boolean,
            required: false,
            default: false,
        }
    },
    components: { },
    setup(props) {
        const store = inject("store");

        // computed
        const formDefinition = computed(() => props.ifRegistrationForm ? 'register' : 'login');
        const loggedUser = computed(() => store.users.state.loggedUser);

        // functions
        const loginUser = (userData) => store.users.actions.loginUser(userData);
        const registerUser = (userData) => store.users.actions.registerUser(userData);

        return { 
            formDefinition,
            loggedUser,
            loginUser,
            registerUser,
        }
        
    },
    methods: {
        async sendRequest(ifRegistrationForm) {
            let logged;
            const email = document.getElementById(`${this.formDefinition}-email`).value;
            const password = document.getElementById(`${this.formDefinition}-password`).value;
            const userData = {
                "Email": email,
                "PwHash": password
            };
            console.log("userdata", userData)
            if (ifRegistrationForm) {
                const username = document.getElementById(`${this.formDefinition}-username`).value;
                userData.UserName = username;
                await this.registerUser(userData);
            } else {
                await this.loginUser(userData);
            }
            if (this.loggedUser.value != 0) {
                this.$router.push({path: '/'});
            }
        }
    }
}
</script>
<style lang="scss">
[class$="-form"] {
    display: inline-block;
    height: max-content;
    text-align: left;

    form {
        display: flex;
        flex-direction: column;
        justify-content: space-around;
        margin-bottom: 200px;
        padding: 20px;
        width: 25vw;
        height: 40vh;
        background: #f5f5fa;
        border: 0;
        border-radius: 8px;
        box-shadow: -10px -10px 30px 0 #fff,10px 10px 30px 0 #1d0dca17;

        
        > * {
            padding: 1em;
            display: block;

            label { 
                text-align: center;
                margin: 0 0 1em 1em;
            }

            input { 
                width: 100%;
                padding:10px;
                border: 0;
                border-radius: 10px;
                box-shadow:0 0 15px 4px rgba(0,0,0,0.06);
            }

            &.submit-btn {
                align-self: center;

                input { 
                    border: 1px;
                    background-color: #fcd997;
                    transition: transform .2s;
                }

                input:hover {
                    transform: scale(1.1);
                }
            }
        }
    }


}
</style>