<template>
    <div :class="[ifRegistrationForm ? 'registration-form' : 'login-form']">
        <div class="form">
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
        </div>
    </div>
</template>
<script>
import { computed, inject } from "vue";
import { useRouter } from 'vue-router'
import { useSidebar, useUsers } from "@/compositionStore/index"

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
        const router = useRouter()
        const { selectSidebar } = useSidebar()
        const { getLoggedInUser } = useUsers()

        // computed
        const formDefinition = computed(() => props.ifRegistrationForm ? 'register' : 'login');
        const loggedUser = getLoggedInUser()

        // functions
        const loginUser = (userData) => store.users.actions.loginUser(userData);
        const registerUser = (userData) => store.users.actions.registerUser(userData);

        return { 
            router,
            formDefinition,
            loggedUser,
            loginUser,
            registerUser,
            selectSidebar
        }
        
    },
    methods: {
        async sendRequest(ifRegistrationForm) {
            const email = document.getElementById(`${this.formDefinition}-email`).value;
            const password = document.getElementById(`${this.formDefinition}-password`).value;
            const userData = {
                "Email": email,
                "PwHash": password
            };

            if (ifRegistrationForm) {
                const username = document.getElementById(`${this.formDefinition}-username`).value;
                userData.UserName = username;
                await this.registerUser(userData);
            } else {
                await this.loginUser(userData);
            }

            if (this.loggedUser && this.loggedUser != 0) {
                this.selectSidebar("User profile/create twit")
                this.router.push({path: '/user-profile'});
            } 
        }
    },
}
</script>
<style lang="scss">
[class$="-form"] {
    display: inline-block;
    height: max-content;
    text-align: left;

    .form {
        
        > * {
            padding: 1rem;
            display: block;

            label { 
                text-align: center;
                margin: 0 0 1em 1rem;
            }

            input { 
                width: 100%;
                padding:10px;
                border: 0;
                border-radius: 10px;
                box-shadow:0 0 15px 4px rgba(0,0,0,0.06);
            }
        }
    }


}
</style>