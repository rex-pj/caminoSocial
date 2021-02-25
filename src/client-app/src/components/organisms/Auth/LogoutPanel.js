import React from "react";
import { PageInfo } from "../../../utils/Constants";
import AuthBanner from "../Banner/AuthBanner";

export default () => {
  return (
    <div className="row g-0">
      <div className="col col-12">
        <AuthBanner
          imageUrl={`${process.env.PUBLIC_URL}/images/logo.png`}
          title={`Logout successfully ${PageInfo.BrandName}`}
          instruction="See you again"
        />
      </div>
    </div>
  );
};
