import { UploadResponse, ExamSummary, GradingResult, MathEvaluationRequest, MathEvaluationResponse, LoginRequest, LoginResponse } from './types';

const API_BASE = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5087/api';

// Auth helpers
export const getToken = (): string | null => {
  if (typeof window === 'undefined') return null;
  return localStorage.getItem('token');
};

export const setToken = (token: string) => {
  if (typeof window !== 'undefined') {
    localStorage.setItem('token', token);
  }
};

export const removeToken = () => {
  if (typeof window !== 'undefined') {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    localStorage.removeItem('userRole');
  }
};

export const setUserInfo = (userId: string, role: string) => {
  if (typeof window !== 'undefined') {
    localStorage.setItem('userId', userId);
    localStorage.setItem('userRole', role);
  }
};

export const getUserInfo = (): { userId: string | null; role: string | null } => {
  if (typeof window === 'undefined') return { userId: null, role: null };
  return {
    userId: localStorage.getItem('userId'),
    role: localStorage.getItem('userRole'),
  };
};

export const isAuthenticated = (): boolean => {
  return getToken() !== null;
};

// Auth API
export const login = async (userId: string, password: string): Promise<LoginResponse> => {
  const response = await fetch(`${API_BASE}/auth/login`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ userId, password } as LoginRequest),
  });

  if (!response.ok) {
    const error = await response.text();
    throw new Error(error || 'Login failed');
  }

  const data = await response.json();
  setToken(data.token);
  setUserInfo(data.userId, data.role);
  return data;
};

export const logout = () => {
  removeToken();
};

export const uploadXml = async (file: File): Promise<UploadResponse> => {
  const token = getToken();
  const formData = new FormData();
  formData.append('file', file);

  const response = await fetch(`${API_BASE}/grading/upload`, {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${token}`,
    },
    body: formData,
  });

  if (!response.ok) {
    const error = await response.text();
    throw new Error(error || 'Failed to upload XML');
  }

  return response.json();
};

export const getStudentAnalytics = async (studentId: number): Promise<ExamSummary[]> => {
  const response = await fetch(`${API_BASE}/analytics/student/${studentId}`);

  if (!response.ok) {
    throw new Error('Failed to fetch student analytics');
  }

  return response.json();
};

export const getExamSummary = async (examId: number): Promise<ExamSummary> => {
  const response = await fetch(`${API_BASE}/analytics/exam/${examId}`);

  if (!response.ok) {
    throw new Error('Failed to fetch exam summary');
  }

  return response.json();
};

export const getExamDetails = async (examId: number): Promise<GradingResult[]> => {
  const response = await fetch(`${API_BASE}/analytics/exam/${examId}/details`);

  if (!response.ok) {
    throw new Error('Failed to fetch exam details');
  }

  return response.json();
};

export const evaluateMath = async (expression: string): Promise<MathEvaluationResponse> => {
  const response = await fetch(`${API_BASE}/math/evaluate`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ expression } as MathEvaluationRequest),
  });

  if (!response.ok) {
    throw new Error('Failed to evaluate expression');
  }

  return response.json();
};
