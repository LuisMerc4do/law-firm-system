import React from "react";
import { NavigationMenuBar } from "./NavBar";
import ActionButtons from "./ActionButtons";
import Logo from "./Logo";

type Props = {};

const Header = (props: Props) => {
  return (
    <div
      className="
    flex
    justify-between
    items-center px-10 border-b h-20
    
    "
    >
      <Logo />
      <NavigationMenuBar />
      <ActionButtons />
    </div>
  );
};

export default Header;
