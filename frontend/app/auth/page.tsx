'use client';

import { FormEvent, useState } from 'react'
import { useRouter } from 'next/navigation';
import RoleSelector from '../components/RoleSelector';
import * as yup from 'yup';
import YupPassword from 'yup-password';
YupPassword(yup)

const registerSchema = yup.object({
    username: yup.string().required("Username is required"),
    email: yup.string().email("Please enter a valid email").required("Email is required"),
    password: yup.string().password()
        .minSymbols(1, "Password must include at least one special character")
        .minUppercase(1, "Password must include at least one uppercase letter")
        .minLowercase(1, "Password must include at least one lowercase letter")
        .minNumbers(1, "Password must include at least one lowercase letter")
        .min(12, "Password must be at least 12 characters long")
        .required("Password is required"),
    role: yup.string().required("Please select a role")
});

const loginSchema = yup.object({
    username: yup.string().required("Username is required"),
    password: yup.string().required("Password is required"),
});

const validationSchema = (isLogin: boolean) => isLogin ? loginSchema : registerSchema;

interface AuthResponse {
    token: string;
    userName: string;
    email: string;
    role: string;
  }

export default function Auth() {
    const [isLogin, setIsLogin] = useState<boolean>(true);
    const [username, setUsername] = useState<string>("");
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [role, setRole] = useState<string>("");
    const [error, setError] = useState<string>("");

    const router = useRouter();

    const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setError("");

        const formData = {
            username,
            email: isLogin ? undefined : email,
            password,
            role: isLogin ? undefined : role,
        }

        try {
            await validationSchema(isLogin).validate(formData, { abortEarly: false });

            const url = isLogin ? 'http://localhost:5156/api/account/login' : 'http://localhost:5156/api/account/register';
            const body = isLogin
                ? JSON.stringify({ username, password })
                : JSON.stringify({ username, email, password, role});

                const response = await fetch(url, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: body
                });

                if (!response.ok) {
                    const errorData = await response.json();
                    throw new Error(errorData.error || 'An error occurred');
                }

                const data: AuthResponse = await response.json();

                if (isLogin) {
                    localStorage.setItem('token', data.token);
                } else {
                    setError('Registration successful. Please log in.');
                    setIsLogin(true);
                }
        } catch (error) {
            if (error instanceof Error){
                setError(error.message);
            } else {
                setError("An error occurred");
            }
        }
    }

    return (
    <div className="flex justify-center items-center min-h-screen bg-gray-100">
      <div className="w-full max-w-md">
        <form onSubmit={handleSubmit} className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
          <h2 className="mb-6 text-2xl font-bold text-center">
            {isLogin ? 'Login' : 'Register'}
          </h2>
          {error && <p className="text-red-500 text-xs italic mb-4">{error}</p>}
          <div className="mb-4">
            <input
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              type="text"
              placeholder="Username"
              value={username}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) => setUsername(e.target.value)}
              required
            />
          </div>
          {!isLogin && (
            <div className="mb-4">
              <input
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                type="email"
                placeholder="Email"
                value={email}
                onChange={(e: React.ChangeEvent<HTMLInputElement>) => setEmail(e.target.value)}
                required
              />
            </div>
          )}
          <div className="mb-6">
            <input
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 mb-3 leading-tight focus:outline-none focus:shadow-outline"
              type="password"
              placeholder="Password"
              value={password}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) => setPassword(e.target.value)}
              required
            />
          </div>
          {!isLogin && (
            <div className="mb-6">
             <RoleSelector role={role} setRole={setRole} />
            </div>
          )}
          <div className="flex items-center justify-between">
            <button
              className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
              type="submit"
            >
              {isLogin ? 'Sign In' : 'Sign Up'}
            </button>
            <a
              className="inline-block align-baseline font-bold text-sm text-blue-500 hover:text-blue-800 cursor-pointer"
              onClick={() => setIsLogin(!isLogin)}
            >
              {isLogin ? 'Need an account?' : 'Already have an account?'}
            </a>
          </div>
        </form>
      </div>
    </div>
    )
}