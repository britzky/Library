"use client";
import Link from "next/link";
import Image from "next/image"

export const Navbar = () => {

  return (
    <div className="w-full">
        <nav className="container relative flex flex-wrap items-center justify-between p-8 mx-auto lg:justify-between xl:px-0">
            <div className="flex flex-wrap items-center justify-between w-full lg:w-auto">
                <Link href="/">
                    <span className="flex items-center space-x-2 text-2xl font-medium text-indigo-500">
                    <span>
                        <Image
                        src="/booklogo.png"
                        alt="book"
                        width="32"
                        height="32"
                        className="w-8"
                        />
                    </span>
                    <span>Library</span>
                    </span>
                </Link>
            </div>

            <div className="hidden mr-3 space-x-4 lg:flex nav__item">
                <Link href="/" className="px-6 py-2 text-white bg-indigo-600 rounded-md md:ml-5">
                    Sign Up
                </Link>

                <Link href="/" className="px-6 py-2 text-white bg-indigo-600 rounded-md md:ml-5">
                    Sign In
                </Link>
            </div>
        </nav>
    </div>
  );
}