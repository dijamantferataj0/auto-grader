import Link from 'next/link';

export default function Home() {
  return (
    <div className="container mx-auto px-4 py-12">
      <div className="max-w-4xl mx-auto text-center">
        <h1 className="text-5xl font-bold mb-6">Auto Grader System</h1>
        <p className="text-xl text-slate-600 mb-12">
          Automatically grade math tests with precision and generate detailed analytics
        </p>

        <div className="grid md:grid-cols-2 gap-8 mb-12">
          <div className="bg-white p-8 rounded-lg shadow-lg">
            <div className="text-6xl mb-4">👨‍🏫</div>
            <h2 className="text-2xl font-bold mb-4">For Teachers</h2>
            <p className="text-slate-600 mb-6">
              Upload XML files containing student exams and receive instant grading results with detailed analytics
            </p>
            <Link
              href="/teacher"
              className="inline-block bg-blue-600 text-white px-8 py-3 rounded-lg font-semibold hover:bg-blue-700 transition-colors"
            >
              Go to Teacher Dashboard
            </Link>
          </div>

          <div className="bg-white p-8 rounded-lg shadow-lg">
            <div className="text-6xl mb-4">👨‍🎓</div>
            <h2 className="text-2xl font-bold mb-4">For Students</h2>
            <p className="text-slate-600 mb-6">
              View your exam results, see which tasks you got correct, and track your progress over time
            </p>
            <Link
              href="/student"
              className="inline-block bg-green-600 text-white px-8 py-3 rounded-lg font-semibold hover:bg-green-700 transition-colors"
            >
              See Analytics
            </Link>
          </div>
        </div>

        <div className="bg-blue-50 border-2 border-blue-200 rounded-lg p-8">
          <h3 className="text-2xl font-bold mb-4">Features</h3>
          <div className="grid md:grid-cols-3 gap-6 text-left">
            <div>
              <div className="text-3xl mb-2">⚡</div>
              <h4 className="font-semibold mb-2">Instant Grading</h4>
              <p className="text-sm text-slate-600">
                Upload XML and get results in seconds
              </p>
            </div>
            <div>
              <div className="text-3xl mb-2">📊</div>
              <h4 className="font-semibold mb-2">Detailed Analytics</h4>
              <p className="text-sm text-slate-600">
                View performance metrics and task-level details
              </p>
            </div>
            <div>
              <div className="text-3xl mb-2">🔢</div>
              <h4 className="font-semibold mb-2">Math Evaluation API</h4>
              <p className="text-sm text-slate-600">
                Independent API for third-party integrations
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
