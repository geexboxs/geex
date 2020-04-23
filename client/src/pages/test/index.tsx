import React, { useState, useEffect, useCallback } from 'react';

export default () => {
  const [count, setCount] = useState(0);

  const getFetchUrl = useCallback(() => {
    console.log(count);
  }, [count]);

  useEffect(() => {
    getFetchUrl();
  }, [getFetchUrl]);

  useEffect(() => {
    setCount(count + 1);
  }, []);

  return <h1>{count}</h1>;
};
