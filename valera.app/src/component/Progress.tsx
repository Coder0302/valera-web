export function Progress({ label, value }: { label: string; value: number }) {
    const v = Math.max(0, Math.min(100, value));
    return (
        <div className="stat">
            <div className="stat-row">
                <span className="stat-label">{label}</span>
                <span className="stat-value">{value}</span>
            </div>
            <div className="bar">
                <div className="bar-fill" style={{ width: `${v}%` }} />
            </div>
        </div>
    );
}
