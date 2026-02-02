'use client';

import Link from 'next/link';
import { usePathname, useRouter } from 'next/navigation';
import { useEffect, useState } from 'react';
import { isAuthenticated, logout } from '@/lib/api';

export default function Navigation() {
  const pathname = usePathname();
  const router = useRouter();
  const [authenticated, setAuthenticated] = useState(false);

  useEffect(() => {
    setAuthenticated(isAuthenticated());
  }, [pathname]);

  const isActive = (path: string) => pathname === path;

  const handleLogout = () => {
    logout();
    setAuthenticated(false);
    router.push('/');
  };

  return (
    <nav className="bg-slate-900 text-white shadow-lg">
      <div className="container mx-auto px-4">
        <div className="flex items-center justify-between h-16">
          <Link href="/" className="text-xl font-bold">
            Auto Grader System
          </Link>

          <div className="flex space-x-4">
            <Link
              href="/"
              className={`px-4 py-2 rounded-lg transition-colors ${
                isActive('/') ? 'bg-blue-600' : 'hover:bg-slate-700'
              }`}
            >
              Home
            </Link>
            <Link
              href="/teacher"
              className={`px-4 py-2 rounded-lg transition-colors ${
                isActive('/teacher') ? 'bg-blue-600' : 'hover:bg-slate-700'
              }`}
            >
              Teacher
            </Link>
            {authenticated ? (
              <button
                onClick={handleLogout}
                className="px-4 py-2 rounded-lg transition-colors hover:bg-slate-700"
              >
                Logout
              </button>
            ) : (
              <Link
                href="/login"
                className={`px-4 py-2 rounded-lg transition-colors ${
                  isActive('/login') ? 'bg-blue-600' : 'hover:bg-slate-700'
                }`}
              >
                Login
              </Link>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
}
