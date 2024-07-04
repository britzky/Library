'use client';

import { useAuth } from "../contexts/AuthContext";
import { redirect } from "next/navigation";
import { useEffect } from "react";

export function withAuth<P extends object>(
    WrappedComponent: React.ComponentType<P>
) {
    return function WithAuth(props: P) {
        const { user, isLoading } = useAuth();

        useEffect(() => {
            if(!isLoading && !user) {
                redirect('/login');
            }
        }, [user, isLoading]);

        if (isLoading){
            return <div>Loading...</div>;
        }

        return user ? <WrappedComponent {...props} /> : null;
    }
}