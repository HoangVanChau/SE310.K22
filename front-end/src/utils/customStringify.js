const customStringify = function(v, replace = null, space = null) {
  const cache = new Set();
  return JSON.stringify(v, function(key, value) {
    if (typeof value === 'object' && value !== null) {
      if (cache.has(value)) {
        // Circular reference found
        try {
          // If this value does not reference a parent it can be deduped
          return JSON.parse(JSON.stringify(value, replace, space));
        } catch (err) {
          // discard key if value cannot be deduped
          return;
        }
      }
      // Store value in our set
      cache.add(value);
    }
    return value;
  });
};
export default customStringify;
