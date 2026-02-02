'use client';

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import TaskDetailsList from '@/components/TaskDetailsList';
import { getExamDetails, getExamSummary } from '@/lib/api';
import { ExamSummary, GradingResult } from '@/lib/types';

export default function AnalyticsPage() {
  const params = useParams();
  const router = useRouter();
  const examId = parseInt(params.examId as string);

  const [summary, setSummary] = useState<ExamSummary | null>(null);
  const [results, setResults] = useState<GradingResult[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const [summaryData, resultsData] = await Promise.all([
          getExamSummary(examId),
          getExamDetails(examId)
        ]);
        setSummary(summaryData);
        setResults(resultsData);
      } catch (err: any) {
        setError(err.message || 'Failed to load exam details');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [examId]);

  if (loading) {
    return (
      <div className="container mx-auto px-4 py-12">
        <div className="text-center">
          <div className="text-4xl mb-4">⏳</div>
          <p className="text-xl">Loading exam details...</p>
        </div>
      </div>
    );
  }

  if (error || !summary) {
    return (
      <div className="container mx-auto px-4 py-12">
        <div className="bg-red-50 border-2 border-red-200 rounded-lg p-8 text-center">
          <div className="text-4xl mb-4">❌</div>
          <h2 className="text-2xl font-bold mb-2">Error</h2>
          <p className="text-red-600">{error || 'Failed to load exam details'}</p>
        </div>
      </div>
    );
  }

  const getScoreColor = (percentage: number) => {
    if (percentage >= 80) return 'text-green-600';
    if (percentage >= 50) return 'text-yellow-600';
    return 'text-red-600';
  };

  return (
    <div className="container mx-auto px-4 py-12">
      <div className="max-w-4xl mx-auto">
        <button
          onClick={() => router.back()}
          className="mb-6 text-blue-600 hover:text-blue-800 font-semibold"
        >
          ← Back
        </button>

        <h1 className="text-4xl font-bold mb-2">Exam Details</h1>
        <p className="text-xl text-slate-600 mb-8">
          Exam #{examId} • Student ID: {summary.studentId}
        </p>

        <div className="bg-white p-6 rounded-lg shadow-md mb-8">
          <div className="grid grid-cols-2 md:grid-cols-4 gap-6">
            <div>
              <p className="text-sm text-slate-600 mb-2">Total Tasks</p>
              <p className="text-3xl font-bold">{summary.totalTasks}</p>
            </div>
            <div>
              <p className="text-sm text-slate-600 mb-2">Correct</p>
              <p className="text-3xl font-bold text-green-600">{summary.correctTasks}</p>
            </div>
            <div>
              <p className="text-sm text-slate-600 mb-2">Incorrect</p>
              <p className="text-3xl font-bold text-red-600">
                {summary.totalTasks - summary.correctTasks}
              </p>
            </div>
            <div>
              <p className="text-sm text-slate-600 mb-2">Score</p>
              <p className={`text-3xl font-bold ${getScoreColor(summary.scorePercentage)}`}>
                {summary.scorePercentage.toFixed(1)}%
              </p>
            </div>
          </div>

          <div className="mt-6">
            <div className="w-full bg-slate-200 rounded-full h-4">
              <div
                className={`h-4 rounded-full transition-all ${
                  summary.scorePercentage >= 80
                    ? 'bg-green-500'
                    : summary.scorePercentage >= 50
                    ? 'bg-yellow-500'
                    : 'bg-red-500'
                }`}
                style={{ width: `${summary.scorePercentage}%` }}
              ></div>
            </div>
          </div>
        </div>

        <h2 className="text-2xl font-semibold mb-6">Task Results</h2>
        <TaskDetailsList results={results} />
      </div>
    </div>
  );
}
