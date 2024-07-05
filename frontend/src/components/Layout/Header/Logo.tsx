import React from "react";
import { Link } from "react-router-dom";

type Props = {};

const Logo = (props: Props) => {
  return (
    <div>
      <Link to="/">
        <img src="/images/logo.png" width={200} height={150} alt="logo" />
      </Link>
    </div>
  );
};

export default Logo;
