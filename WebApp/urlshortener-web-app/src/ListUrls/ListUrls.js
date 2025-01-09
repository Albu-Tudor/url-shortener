import React from "react";

function ListUrls({urls}) {
    return (
        <div>
            {urls && urls.map((url) => (
                <li key={url.shortUrl}>
                    <a href={url.shortUrl} target="_blank" rel="noopener noreferrer">{url.shortUrl}</a>
                    {' → '}
                    <a href={url.longUrl} target="_blank" rel="noopener noreferrer">{url.shortUrl}</a>
                </li>
            ))}
        </div>
    );
}

export default ListUrls;
